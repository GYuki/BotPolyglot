using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LogicBlock.Translations.Extensions;
using LogicBlock.Translations.Model;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Polly;
using Polly.Retry;

namespace LogicBlock.Translations.Infrastructure
{
    public class TranslationContextSeed
    {
        public async Task SeedAsync(TranslationContext context, string contentRootPath, ILogger<TranslationContextSeed> logger)
        {
            var policy = CreatePolicy(logger, nameof(TranslationContextSeed));

            await policy.ExecuteAsync(async () => 
            {

                if (!context.Words.Any()) // ONLY FOR FIRST LAUNCH
                {
                    await context.Words.AddRangeAsync(
                        GetWords(1000)
                    );
                    
                    await context.SaveChangesAsync();
                }

                if (!context.Set<EnLanguage>().Any())
                {
                    var englishSet = GetEnglishTranslations(contentRootPath, context, logger);
                    if (englishSet != null)
                        await context.Set<EnLanguage>().AddRangeAsync(
                            englishSet  
                        );
                    await context.SaveChangesAsync();
                }
            });
        }

        private IEnumerable<Word> GetWords(int count)
        {
            return Enumerable.Range(1, count).Select(x => new Word{
                        Id = x
                    });
        }

        private IEnumerable<EnLanguage> GetEnglishTranslations(string contentRootPath, TranslationContext context, ILogger<TranslationContextSeed> logger)
        {
            string csvFileEnglishTranslations = Path.Combine(contentRootPath, "Setup", "English.csv");

            if (!File.Exists(csvFileEnglishTranslations))
                return null;

            string[] csvHeaders;
            try
            {
                string[] requiredHeaders = { "translation", "wordid" };
                csvHeaders = GetHeaders(csvFileEnglishTranslations, requiredHeaders);
            }
            catch (Exception e)
            {
                logger.LogError(e, "EXCEPTION ERROR: {Message}", e.Message);
                return null;
            }

            var wordIdsLookup = context.Words.Select(x => x.Id).ToArray();
    
            return File.ReadAllLines(csvFileEnglishTranslations)
                                        .Skip(1)
                                        .Select(row => Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                                        .SelectTry(column => CreateTranslation<EnLanguage>(column, csvHeaders, wordIdsLookup))
                                        .OnCaughtException(ex => { logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
                                        .Where(x => x != null);
        }

        private TLanguage CreateTranslation<TLanguage>(string[] column, string[] headers, int[] wordIds) where TLanguage : AbstractLanguage, new()
        {
            if (column.Length != headers.Length)
                throw new Exception($"column count '{column.Count()}' not the same as headers count'{headers.Count()}'");

            string translation = column[Array.IndexOf(headers, "translation")].Trim('"').Trim();
            string wordIdString = column[Array.IndexOf(headers, "wordid")].Trim('"').Trim();

            if (!Int32.TryParse(wordIdString, out int wordId))
                throw new Exception($"wordid={wordIdString} is not valid integer");
            
            if (!wordIds.Contains(wordId))
                throw new Exception($"wordId={wordId} does not exist in Words");
            
            return new TLanguage
            {
                Translation = translation,
                WordId = wordId
            };
        }

        private string[] GetHeaders(string csvfile, string[] requiredHeaders, string[] optionalHeaders = null)
        {
            string[] csvheaders = File.ReadLines(csvfile).First().ToLowerInvariant().Split(',');

            if (csvheaders.Count() < requiredHeaders.Count())
            {
                throw new Exception($"requiredHeader count '{ requiredHeaders.Count()}' is bigger then csv header count '{csvheaders.Count()}' ");
            }

            if (optionalHeaders != null)
            {
                if (csvheaders.Count() > (requiredHeaders.Count() + optionalHeaders.Count()))
                {
                    throw new Exception($"csv header count '{csvheaders.Count()}'  is larger then required '{requiredHeaders.Count()}' and optional '{optionalHeaders.Count()}' headers count");
                }
            }

            foreach (var requiredHeader in requiredHeaders)
            {
                if (!csvheaders.Contains(requiredHeader))
                {
                    throw new Exception($"does not contain required header '{requiredHeader}'");
                }
            }

            return csvheaders;
        }

        private AsyncRetryPolicy CreatePolicy(ILogger<TranslationContextSeed> logger, string prefix, int retries=3)
        {
            return Policy.Handle<MySqlException>()
                .WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        logger.LogWarning(exception, "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}", prefix, exception.GetType().Name, exception.Message, retry, retries);
                    }
                );
        }
    }
}