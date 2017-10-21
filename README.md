# weddingslideshow
Displays a fullscreen slideshow of images for a wedding.

If you'd like to use this you'll need to register with the Flickr API, set it up and instruct guests at your wedding to use the flickr app with an agreed hashtag. 

[Contributions](CONTRIBUTIONS.md) welcome. If you want some help reach out via issues.

## app.js
A vanilla javascript/html application that I deploy to S3 websites fronted by cloudfront. 

[2015] Initially this used the Instagram API directly. In 2015 Instagram removed searching for images by hashtag so this stopped working. 

[2017] For a friends wedding an API was introduced to call the Flickr API to search by hashtag.   

## weddingslideshow.api
This is a netcore 1.0 app that connects to the flickr API to pull down images by hashtag. It is deployed to AWS Lambda and fronted by API gateway.

[2017] Api introduced

## Local development 
### Local development commands
Run these commands inside the /api directory
* `dotnet build` - compiles the solution
* `dotnet test` - runs the tests

## Configuration
You'll need to configure the API in order to run it. 
Create two files for local vs production configuration:
`/api/weddingslideshow.api/appsettings.Development.json`
`/api/weddingslideshow.api/appsettings.Production.json`

### Configuration structure
Each file should contain a JSON object similar to the following:
`{
  "AWS": { 
    "Region": "us-west-2"
  },
  "FlickrAPI": {
    "ApiKey": "[YOUR-FLICKR-KEY]",
    "ApiSecret": "[YOUR-FLICKR-KEY]",
    "Uri": "https://api.flickr.com/services/rest/"    
  }
}`

### Flickr Credentials
You'll need to register with Flickr for an API key and secret. See the [Flickr API documentation](https://www.flickr.com/services/api/misc.overview.html) for more information.

## Deployment
### API publish via AWS SAM to cloudformation, api gateway, S3 and lambda
Run these commands inside the /api/weddingslideshow.api directory
* `dotnet lambda deploy-serverless` - publishes the infrastructure to AWS with cloudformation. Requires IAM credentials in a profile named lukeryannetnz.

### Front end publish to S3
Run these commands inside the root directory
* `aws s3 cp . s3://[BUCKETNAME] --recursive --exclude "*/**.*" --exclude ".git/*" --exclude ".DS_Store" --exclude ".gitignore" --exclude ".gitattributes"` - publishes the front end to an S3 bucket. Requires IAM credentials in the default profile.
