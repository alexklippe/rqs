$(document).ready(function () {
	$('#search').click(function () {
	
		$.ajax({
			type: 'GET',
			url: "/api/v1/requirement/",
			data: "{}",
			contentType: "application/json; charset=utf-8",
			async: true,
			dataType: "json",
			cache: false,
		  
			success: function (json) {
				console.log(json);
				var tr;
				for (var i = 0; i < json.length; i++) {
					tr = $('<tr/>');
					tr.append("<td>" + json[i].ID + "</td>");
					tr.append("<td>" + json[i].TMSTask + "</td>");
					tr.append("<td>" + json[i].CCP + "</td>");
					tr.append("<td>" + json[i].Text + "</td>");
					$('#searchResults').append(tr);
				}
			},
			error: function (msg) {
				alert(msg.responseText);
			}
		});
	});
});


