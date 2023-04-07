using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace anything.Components
{
    public class Events
    {
        public static async Task SpamMessage(string token, string content_sacrifice, string channel_id, int amount)
        {
            var threads = new List<Thread>();

            for (int i = 0; i < amount; i++)
            {
                threads.Add(new Thread(async () =>
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Add("Authorization", token);

                        var compile = new { content = content_sacrifice };
                        var compile_turn = JsonConvert.SerializeObject(compile);

                        HttpResponseMessage response = await client.PostAsync($"https://discord.com/api/v9/channels/{channel_id}/messages", new StringContent(compile_turn, Encoding.UTF8, "application/json"));

                        if (response.IsSuccessStatusCode)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Message sent successfully");
                            Console.ResetColor();
                        }
                        else if ((int)response.StatusCode == 429)
                        {
                            var ratelimit = await response.Content.ReadAsStringAsync();
                            var retryTime = JObject.Parse(ratelimit)["retry_after"].Value<float>();

                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Ratelimited. Retry after {retryTime} seconds");

                            await Task.Delay(TimeSpan.FromSeconds(retryTime));
                            await SpamMessage(token, content_sacrifice, channel_id, 1);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Message sending failed with status code {(int)response.StatusCode}");
                            Console.ResetColor();
                        }

                        client.Dispose();
                    }

                }));
            }

            threads.ForEach(thread => thread.Start());
            threads.ForEach(thread => thread.Join());
        }
    }
}