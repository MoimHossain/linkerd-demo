var map, datasource;

var defaultColor = '#2c9cea';
var colorScale = [
    25, '#73ffdc',
    50, '#f9ffb9',
    75, '#ffb849',
    100, '#ca0056'
];

function createMap() {
    //Initialize a map instance.
    map = new atlas.Map('mapctl', {
        zoom: 1,
        view: 'Auto',
        style: 'grayscale_dark',
        authOptions: {
            authType: 'subscriptionKey',
            subscriptionKey: 'tTk1JVEaeNvDkxxnxHm9cYaCvqlOq1u-fXTvyXn2XkA'
        }
    });

    //Wait until the map resources are ready.
    map.events.add('ready', function () {
        //Create a data source to store point data.
        pointDatasource = new atlas.source.DataSource();
        map.sources.add(pointDatasource);

        //Load earthquake data into the point data source. When it completes, copy the points into the gridded data source.
        //pointDatasource.importDataFromUrl(earthquakeFeed).then(() => {
        //    datasource.add(pointDatasource.toJson());
        //});

        //var earthquakeFeed = "https://cloudoven.blob.core.windows.net/covid/CovidData.json";
    
        //fetch(earthquakeFeed)
        //    .then(response => response.json())
        //    .then(data => {
        //        console.log(data)
        //        data.forEach(feature => {

        //            setTimeout(() => {
        //                datasource.add(new atlas.data.Feature(
        //                    new atlas.data.Point([feature.longitude, feature.latitude]),
        //                    {
        //                        "mag": 1
        //                    }));

        //            }, 2000);


        //        });

        //    });

       
         //setTimeout(() => {
         
         //    cvdata.forEach(feature => {

         //        setTimeout(() => {
         //            datasource.add(new atlas.data.Feature(
         //                new atlas.data.Point([feature.longitude, feature.latitude]),
         //                {
         //                    "mag": 1
         //                }));

         //        }, 20);
         //    });
         
         //}, 2000);

 

        //Create an instance of the gridded data source.
        datasource = new atlas.source.GriddedDataSource(null, {
            cellWidth: 300,
            distanceUnits: 'miles'
        });
        map.sources.add(datasource);

        //Create a stepped expression based on the color scale.
        var steppedExp = [
            'step',
            ['get', 'point_count'],
            defaultColor
        ];
        steppedExp = steppedExp.concat(colorScale);

        map.layers.add([
            //Create layers to render the polygon and outline of each grid cell.
            new atlas.layer.PolygonLayer(datasource, null, {
                fillColor: steppedExp,
                fillOpacity: 0.9
            }),

            new atlas.layer.LineLayer(datasource, null, {
                strokeColor: 'white',
                strokeWidth: 1,
            }),

            //Create a layer to render the individual points on the map.
            new atlas.layer.BubbleLayer(pointDatasource, null, {
                radius: 20,
                color: '#ff007f',
                strokeWidth: 0
            })
        ], 'labels');
    });
}

