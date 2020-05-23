using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LogicBlock.Translations.Extensions;
using LogicBlock.Translations.Model.Texts;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using StackExchange.Redis;

public class TextManagerSeed
{
    public async Task SeedAsync(ConnectionMultiplexer redis, string contentRootPath, ILogger<TextManagerSeed> logger)
    {
        var policy = CreatePolicy(logger, nameof(TextManagerSeed));

        await policy.ExecuteAsync(async() => 
        {
            var managerSet = GetManagerTexts(contentRootPath, logger);

            var database = redis.GetDatabase(2);

            foreach(var m in managerSet)
            {
                if (m.key != null && m.text != null)
                    await database.StringSetAsync(m.key, JsonConvert.SerializeObject(m.text));
            }
        });
    }

    private IEnumerable<(string key, Text text)> GetManagerTexts(string contentRootPath, ILogger<TextManagerSeed> logger)
    {
        string csvFile = Path.Combine(contentRootPath, "Setup", "TextManager.csv");

        if (!File.Exists(csvFile))
            return null;

        string[] csvHeaders;
        try
        {
            string[] requiredHeaders = { "key", "russian" };
            csvHeaders = GetHeaders(csvFile, requiredHeaders);
        }
        catch (Exception e)
        {
            logger.LogError(e, "EXCEPTION ERROR: {Message}", e.Message);
            return null;
        }

        return File.ReadAllLines(csvFile)
                                .Skip(1)
                                .Select(row => Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                                .SelectTry(column => CreateText(column, csvHeaders))
                                .OnCaughtException(ex => { logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return (null, null); })
                                .Where(x => x.key != null && x.text != null);
    }

    private (string key, Text text) CreateText(string[] column, string[] headers)
    {
        if (column.Length != headers.Length)
            throw new Exception($"column count '{column.Count()}' not the same as headers count'{headers.Count()}'");
        
        string key = column[Array.IndexOf(headers, "key")].Trim('"').Trim();
        string russian = column[Array.IndexOf(headers, "russian")].Trim('"').Trim();

        return (key, new Text
        {
            Russian = russian
        });
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

    private AsyncRetryPolicy CreatePolicy(ILogger<TextManagerSeed> logger, string prefix, int retries=3)
        {
            return Policy.Handle<RedisException>()
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