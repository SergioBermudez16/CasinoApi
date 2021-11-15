using Casino.Api.Models;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using Xunit;

namespace CasinoApi.Test
{

    public class CasinoControllerTests : IntegrationTestBuilder
    {
        [Fact]
        public void AddRouletteSuccess()
        {
            HttpResponseMessage response = TestClient.PostAsync("api/Roulette", "", new JsonMediaTypeFormatter()).Result;
            response.EnsureSuccessStatusCode();
            var responseCont = response.Content.ReadAsStringAsync().Result;
            var rouletteId = System.Text.Json.JsonSerializer.Deserialize<string>(responseCont);
            Assert.Equal(36, rouletteId.Length);
        }

        [Fact]
        public void OpenRouletteSuccess()
        {
            var postResponse = TestClient.PostAsync("api/Roulette", "", new JsonMediaTypeFormatter()).Result;
            postResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, postResponse.StatusCode);
            var postResponseCont = postResponse.Content.ReadAsStringAsync().Result;
            var rouletteId = System.Text.Json.JsonSerializer.Deserialize<string>(postResponseCont);
            var putResponse = TestClient.PutAsync($"api/Roulette/{rouletteId}", "", new JsonMediaTypeFormatter()).Result;
            putResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);
        }

        [Fact]
        public void GetRouletteResultSuccess()
        {
            var postresponse = TestClient.PostAsync("api/Roulette", "", new JsonMediaTypeFormatter()).Result;
            postresponse.EnsureSuccessStatusCode();
            var postResponseCont = postresponse.Content.ReadAsStringAsync().Result;
            var rouletteId = System.Text.Json.JsonSerializer.Deserialize<string>(postResponseCont);
            var putResponse = TestClient.PutAsync($"api/Roulette/{rouletteId}", "", new JsonMediaTypeFormatter()).Result;
            putResponse.EnsureSuccessStatusCode();
            var firstBet = new
            {
                RouletteId = rouletteId,
                BetType = 1,
                Bet = "36",
                Money = 5000
            };
            var userId = Guid.NewGuid().ToString();
            TestClient.DefaultRequestHeaders.Add("userId", userId);
            var postBetresponse = TestClient.PostAsync("api/RouletteBet", firstBet, new JsonMediaTypeFormatter()).Result;
            postBetresponse.EnsureSuccessStatusCode();
            TestClient.DefaultRequestHeaders.Remove("userId");
            var postBetResponseCont = postBetresponse.Content.ReadAsStringAsync().Result;
            var rouletteBetId = System.Text.Json.JsonSerializer.Deserialize<string>(postBetResponseCont);
            Assert.Equal(36, rouletteBetId.Length);
            var getResponse = TestClient.GetAsync($"api/Roulette/{rouletteId}").Result;
            getResponse.EnsureSuccessStatusCode();
            var getResponseCont = getResponse.Content.ReadAsStringAsync().Result;
            var rouletteResult = System.Text.Json.JsonSerializer.Deserialize<List<RouletteResultDto>>(getResponseCont);
            rouletteResult[0].RouletteId.Should().Be(rouletteId);
            rouletteResult[0].UserId.Should().Be(userId);
        }
    }
}
