var globalUpdateFunc = function () {
    fetch('/Home/GetGlobal')
        .then(response => response.json())
        .then(data => {

            var htmls = [];
            htmls.push('<a class="item">New Confirmed<div class="ui label yellow">' + data.newConfirmed + '</div></a>');
            htmls.push('<a class="item">Total Confirmed<div class="ui label orange">' + data.totalConfirmed + '</div></a>');
            htmls.push('<a class="item">New Deaths<div class="ui label brown">' + data.newDeaths + '</div></a>');
            htmls.push('<a class="item">Total Deaths<div class="ui label red">' + data.totalDeaths + '</div></a>');
            htmls.push('<a class="item">New Recovered<div class="ui label green">' + data.newRecovered + '</div></a>');
            htmls.push('<a class="item">Total Recovered<div class="ui label teal">' + data.totalRecovered + '</div></a>');


            $('#menuContainer').html(htmls.join(' '));

            if (window.globalUpdate) {
                if (window.globalUpdate.date.nanos !== data.date.nanos) {
                    configureAnimation('#menuContainer');
                }
            }
            window.globalUpdate = data;
        });
};

var countryUpdateFunc = function () {
    fetch('/Home/GetAllCountry')
        .then(response => response.json())
        .then(data => {
            var htmls = [];
            data.forEach(row => {
                row.slug = (row.slug + "").replace("-", "");

                htmls.push('<tr id="' + row.slug + '">');
                htmls.push('<td>' + row.countryCountry + '</td>');
                htmls.push('<td class="warning" align="right">' + row.newConfirmed + '</div></a>');
                htmls.push('<td class="warning" align="right">' + row.totalConfirmed + '</div></a>');
                htmls.push('<td class="negative" align="right">' + row.newDeaths + '</div></a>');
                htmls.push('<td class="negative" align="right">' + row.totalDeaths + '</div></a>');
                htmls.push('<td class="positive" align="right">' + row.newRecovered + '</div></a>');
                htmls.push('<td class="positive" align="right">' + row.totalRecovered + '</div></a>');
                htmls.push('</tr>');
            });

            $('#countryBody').html(htmls.join(' '));

            data.forEach(row => {
                if (window[row.slug]) {
                    var old = window[row.slug];
                    if (old && old.date && old.date.nanos !== row.date.nanos) {
                        configureAnimation('#' + row.slug);
                    }
                }
                window[row.slug] = row;
            });
        });
}

function configureAnimation(itemID) {
    var delayBeforeAnim = Math.floor((Math.random() * 1000) + 1);
    setTimeout(() => {
        $(itemID).effect('highlight', { color: '#FFC300' }, 3000);
    }, delayBeforeAnim);
}