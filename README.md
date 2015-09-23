# APIParsers
Simple programs to parse JSON responses from popular APIs.

The CensusAPIParser takes as input a JSON file and outputs a text file as a set of key value pairs separated by semi-colons.  

The GoogleAPIParser takes as input a Excel file that has a list of objects with Latitude and Longitude and outputs a text file places of interest that are within 50000 meters of the Lat and Long you put in.  The type of place and the distance can be modified in the web query.

The GoogleAPIReverseGeoCoder takes as input a Excel file that has a address but is missing a zipcode.  It outputs a text file with the full address and zipcode.
