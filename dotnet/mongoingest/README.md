# Introduction

Small C# Example for reading metadata from azure blobs and saving that metadata in mongodb.

To run the program, create two secrets in the root folder

secret_mongodb
```
mongodb+srv://<username>:<password>@YOURNAMEHERE.mongocluster.cosmos.azure.com/?tls=true&authMechanism=SCRAM-SHA-256&retrywrites=false&maxIdleTimeMS=120000
```

secret_sas
```
https://YOURNAMEHERETOO.blob.core.windows.net?sv=<sas_token>
```

And start the program!

## Search speed results

var filter1 = Builders<BsonDocument>.Filter.And(
    Builders<BsonDocument>.Filter.GeoIntersects("geometry.coordinates", groot),
    Builders<BsonDocument>.Filter.GeoIntersects("geometry.coordinates", nl),
    Builders<BsonDocument>.Filter.GeoIntersects("geometry.coordinates", blokindeling),
    Builders<BsonDocument>.Filter.GeoIntersects("geometry.coordinates", rivierenland1),
    Builders<BsonDocument>.Filter.GeoIntersects("geometry.coordinates", rivierenland2),
    Builders<BsonDocument>.Filter.GeoIntersects("geometry.coordinates", rijnijssel)
)

var filter2 = Builders<BsonDocument>.Filter.Or(
    Builders<BsonDocument>.Filter.GeoIntersects("geometry.coordinates", nl),
    Builders<BsonDocument>.Filter.GeoIntersects("geometry.coordinates", groot),
    Builders<BsonDocument>.Filter.GeoIntersects("geometry.coordinates", limburg),
    Builders<BsonDocument>.Filter.GeoIntersects("geometry.coordinates", friesland),
    Builders<BsonDocument>.Filter.GeoIntersects("geometry.coordinates", zuidholl),
    Builders<BsonDocument>.Filter.GeoIntersects("geometry.coordinates", blokindeling),
    Builders<BsonDocument>.Filter.GeoIntersects("geometry.coordinates", rivierenland1),
    Builders<BsonDocument>.Filter.GeoIntersects("geometry.coordinates", rivierenland2),
    Builders<BsonDocument>.Filter.GeoIntersects("geometry.coordinates", rijnijssel)
)

### 600k records, geospatial index on geometry.coordinates_2dsphere.

Run 10 times
Execution time filter1 is 425 ms. Found 1338 documents
Execution time filter1 is 292 ms. Found 1338 documents
Execution time filter1 is 214 ms. Found 1338 documents
Execution time filter1 is 248 ms. Found 1338 documents
Execution time filter1 is 222 ms. Found 1338 documents
Execution time filter1 is 219 ms. Found 1338 documents
Execution time filter1 is 209 ms. Found 1338 documents
Execution time filter1 is 236 ms. Found 1338 documents
Execution time filter1 is 233 ms. Found 1338 documents
Execution time filter1 is 216 ms. Found 1338 documents
Average time: 251 ms

Run 10 times
Execution time filter2 is 2813 ms. Found 598775 documents
Execution time filter2 is 2780 ms. Found 598775 documents
Execution time filter2 is 2811 ms. Found 598775 documents
Execution time filter2 is 2790 ms. Found 598775 documents
Execution time filter2 is 2822 ms. Found 598775 documents
Execution time filter2 is 2741 ms. Found 598775 documents
Execution time filter2 is 2774 ms. Found 598775 documents
Execution time filter2 is 2728 ms. Found 598775 documents
Execution time filter2 is 2731 ms. Found 598775 documents
Execution time filter2 is 2731 ms. Found 598775 documents
Average time: 2772 ms

Run 10 times
Execution time filter3 is 3776 ms. Found 593333 documents
Execution time filter3 is 3774 ms. Found 593333 documents
Execution time filter3 is 3742 ms. Found 593333 documents
Execution time filter3 is 3737 ms. Found 593333 documents
Execution time filter3 is 3687 ms. Found 593333 documents
Execution time filter3 is 3756 ms. Found 593333 documents
Execution time filter3 is 3693 ms. Found 593333 documents
Execution time filter3 is 3716 ms. Found 593333 documents
Execution time filter3 is 3810 ms. Found 593333 documents
Execution time filter3 is 3792 ms. Found 593333 documents
Average time: ~3700 ms

### 600k records, NO geospatial index

Run 10 times
Execution time filter1 is 4494 ms. Found 1338 documents
Execution time filter1 is 4329 ms. Found 1338 documents
Execution time filter1 is 4297 ms. Found 1338 documents
Execution time filter1 is 4266 ms. Found 1338 documents
Execution time filter1 is 4302 ms. Found 1338 documents
Execution time filter1 is 4260 ms. Found 1338 documents
Execution time filter1 is 4276 ms. Found 1338 documents
Execution time filter1 is 4257 ms. Found 1338 documents
Execution time filter1 is 4336 ms. Found 1338 documents
Execution time filter1 is 4246 ms. Found 1338 documents
Average time: 4306 ms

Run 10 times
Execution time filter2 is 2048 ms. Found 598775 documents
Execution time filter2 is 1961 ms. Found 598775 documents
Execution time filter2 is 1993 ms. Found 598775 documents
Execution time filter2 is 1970 ms. Found 598775 documents
Execution time filter2 is 1951 ms. Found 598775 documents
Execution time filter2 is 1950 ms. Found 598775 documents
Execution time filter2 is 1968 ms. Found 598775 documents
Execution time filter2 is 2088 ms. Found 598775 documents
Execution time filter2 is 1943 ms. Found 598775 documents
Execution time filter2 is 2037 ms. Found 598775 documents
Average time: 1990 ms

Run 10 times
Execution time filter3 is 6554 ms. Found 593333 documents
Execution time filter3 is 6580 ms. Found 593333 documents
Execution time filter3 is 6432 ms. Found 593333 documents
Execution time filter3 is 6436 ms. Found 593333 documents
Execution time filter3 is 6452 ms. Found 593333 documents
Execution time filter3 is 6414 ms. Found 593333 documents
Execution time filter3 is 6439 ms. Found 593333 documents
Execution time filter3 is 6409 ms. Found 593333 documents
Execution time filter3 is 6544 ms. Found 593333 documents
Execution time filter3 is 6454 ms. Found 593333 documents
Average time: ~6400 ms

### Pagination

Niet gesorteerd krijg je dit:

Execution time geoAndProductFilter is 615 ms. Found 174 documents

Ortho/2/22/beelden_ecw_tegels/2023_184000_426000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_185000_426000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_185000_427000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_185000_428000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_184000_428000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_184000_427000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_183000_427000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_183000_428000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_182000_428000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_182000_427000_RGB_hrl.ecw
Ortho/6/5/beelden_CIR_ecw_tegels/2023_182000_430000_CIR_lrl.ecw
Ortho/6/5/beelden_RGB_ecw_tegels/2023_182000_430000_RGB_lrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_182000_426000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_183000_426000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_181000_426000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_180000_426000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_181000_427000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_181000_428000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_181000_429000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_182000_431000_RGB_hrl.ecw
Ortho/6/5/beelden_CIR_ecw_tegels/2023_182000_434000_CIR_lrl.ecw
Ortho/6/5/beelden_RGB_ecw_tegels/2023_182000_434000_RGB_lrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_181000_430000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_181000_431000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_181000_433000_RGB_hrl.ecw
Ortho/5/93/beelden_ecw_tegels/2023_181000_433000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_181000_432000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_182000_432000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_182000_433000_RGB_hrl.ecw
Ortho/5/93/beelden_ecw_tegels/2023_182000_433000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_183000_433000_RGB_hrl.ecw
Ortho/5/93/beelden_ecw_tegels/2023_183000_433000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_184000_433000_RGB_hrl.ecw
Ortho/5/93/beelden_ecw_tegels/2023_184000_433000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_183000_432000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_182000_430000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_182000_429000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_183000_429000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_183000_430000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_183000_431000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_184000_430000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_184000_429000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_185000_429000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_185000_430000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_185000_432000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_185000_431000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_184000_431000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_184000_432000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_185000_433000_RGB_hrl.ecw
Ortho/5/93/beelden_ecw_tegels/2023_185000_433000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_186000_433000_RGB_hrl.ecw
Ortho/5/93/beelden_ecw_tegels/2023_186000_433000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_186000_432000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_186000_431000_RGB_hrl.ecw
Ortho/6/5/beelden_CIR_ecw_tegels/2023_186000_434000_CIR_lrl.ecw
Ortho/6/5/beelden_RGB_ecw_tegels/2023_186000_434000_RGB_lrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_187000_431000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_187000_432000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_187000_433000_RGB_hrl.ecw
Ortho/5/94/beelden_ecw_tegels/2023_187000_433000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_188000_432000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_189000_432000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_189000_431000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_188000_431000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_188000_430000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_189000_430000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_189000_429000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_188000_429000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_187000_429000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_187000_430000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_186000_430000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_186000_429000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_186000_428000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_187000_428000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_186000_427000_RGB_hrl.ecw
Ortho/6/5/beelden_CIR_ecw_tegels/2023_186000_430000_CIR_lrl.ecw
Ortho/6/5/beelden_RGB_ecw_tegels/2023_186000_430000_RGB_lrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_186000_426000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_188000_427000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_188000_426000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_187000_426000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_187000_427000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_188000_428000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_189000_428000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_190000_428000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_190000_427000_RGB_hrl.ecw
Ortho/6/5/beelden_CIR_ecw_tegels/2023_190000_430000_CIR_lrl.ecw
Ortho/6/5/beelden_RGB_ecw_tegels/2023_190000_430000_RGB_lrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_190000_426000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_189000_427000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_189000_426000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_192000_426000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_191000_426000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_191000_427000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_191000_428000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_192000_427000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_192000_428000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_192000_429000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_192000_430000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_191000_429000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_190000_429000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_190000_430000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_191000_430000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_191000_431000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_190000_431000_RGB_hrl.ecw
Ortho/6/5/beelden_CIR_ecw_tegels/2023_190000_434000_CIR_lrl.ecw
Ortho/6/5/beelden_RGB_ecw_tegels/2023_190000_434000_RGB_lrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_190000_432000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_191000_432000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_192000_432000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_192000_431000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_193000_432000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_194000_432000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_194000_431000_RGB_hrl.ecw
Ortho/6/5/beelden_CIR_ecw_tegels/2023_194000_434000_CIR_lrl.ecw
Ortho/6/5/beelden_RGB_ecw_tegels/2023_194000_434000_RGB_lrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_193000_431000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_193000_430000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_193000_429000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_193000_428000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_194000_429000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_194000_428000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_194000_430000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_195000_430000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_195000_429000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_196000_429000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_196000_430000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_196000_431000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_195000_431000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_195000_432000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_196000_432000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_197000_432000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_197000_431000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_197000_430000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_198000_431000_RGB_hrl.ecw
Ortho/6/5/beelden_CIR_ecw_tegels/2023_198000_434000_CIR_lrl.ecw
Ortho/6/5/beelden_RGB_ecw_tegels/2023_198000_434000_RGB_lrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_198000_430000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_198000_432000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_199000_432000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_200000_432000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_200000_431000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_200000_430000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_199000_430000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_199000_431000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_198000_429000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_197000_429000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_193000_427000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_193000_426000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_201000_432000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_199000_433000_RGB_hrl.ecw
Ortho/5/94/beelden_ecw_tegels/2023_199000_433000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_198000_433000_RGB_hrl.ecw
Ortho/5/94/beelden_ecw_tegels/2023_198000_433000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_197000_433000_RGB_hrl.ecw
Ortho/5/94/beelden_ecw_tegels/2023_197000_433000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_196000_433000_RGB_hrl.ecw
Ortho/5/94/beelden_ecw_tegels/2023_196000_433000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_195000_433000_RGB_hrl.ecw
Ortho/5/94/beelden_ecw_tegels/2023_195000_433000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_194000_433000_RGB_hrl.ecw
Ortho/5/94/beelden_ecw_tegels/2023_194000_433000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_193000_433000_RGB_hrl.ecw
Ortho/5/94/beelden_ecw_tegels/2023_193000_433000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_192000_433000_RGB_hrl.ecw
Ortho/5/94/beelden_ecw_tegels/2023_192000_433000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_191000_433000_RGB_hrl.ecw
Ortho/5/94/beelden_ecw_tegels/2023_191000_433000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_190000_433000_RGB_hrl.ecw
Ortho/5/94/beelden_ecw_tegels/2023_190000_433000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_189000_433000_RGB_hrl.ecw
Ortho/5/94/beelden_ecw_tegels/2023_189000_433000_RGB_hrl.ecw
Ortho/2/22/beelden_ecw_tegels/2023_188000_433000_RGB_hrl.ecw
Ortho/5/94/beelden_ecw_tegels/2023_188000_433000_RGB_hrl.ecw
