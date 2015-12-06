if(window.location.pathname.match(/\/Battles\/Info\/\d+/)) {
	(function () {
		var name = document.getElementsByTagName("h1")[0].innerText;

		var table = document.getElementsByTagName("table")[0].getElementsByTagName("tbody")[0].children;

		var result = {};
		result["First"] = {};
		result["First"].GamesWon = 0;
		result["First"].HandsPlayed = 0;

		result["Second"] = {};
		result["Second"].GamesWon = 0;
		result["Second"].HandsPlayed = 0;

		result["Name"] = name;

		for (var i = 1; i < table.length; i++) {
			var cols =  table[i].getElementsByTagName("td");
			var winner = cols[1].innerText;

			var cards = cols[2].innerText;
			cards = cards.match(/\W+[0-9]+/)[0].trim();

			if (winner === "First") {
				result["First"].HandsPlayed += cards * 1;
				result["First"].GamesWon += 1;
			}
			else
			{
				result["Second"].HandsPlayed += cards * 1;
				result["Second"].GamesWon += 1;
			}
		}

		result["First"].AverageHandsPer = result["First"].HandsPlayed / result["First"].GamesWon;
		result["Second"].AverageHandsPer = result["Second"].HandsPlayed / result["Second"].GamesWon;

		console.log(result.Name);
		console.log("Games won by First: " + result.First.GamesWon);
		console.log("Hands played when won by First: " + result.First.HandsPlayed);
		console.log("Average hands played when won by First: " + result.First.AverageHandsPer.toFixed(2));
		console.log("---------------------------------------")
		console.log("Games won by Second: " + result.Second.GamesWon);
		console.log("Hands played when won by Second: " + result.Second.HandsPlayed);
		console.log("Average hands played when won by Second: " + result.Second.AverageHandsPer.toFixed(2));
	// return result;
}());
}