# weddingslideshow
Javascript application that displays a fullscreen slideshow of images for a wedding.

# weddingslideshow.api
This is a netcore 1.0 app that connects to the flickr API to pull down images by hashtag.

## Local development commands
Run these commands inside the ~/weddingslideshow.api directory
* `dotnet build` - compiles the solution
* `dotnet test` - runs the tests

## Configuration
You'll need to configure the API in order to run it. 
Create two files for local vs production configuration:
/weddingslideshow.api/weddingslideshow.api/appsettings.Development.json
/weddingslideshow.api/weddingslideshow.api/appsettings.Production.json

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
Run these commands inside the ~/weddingslideshow.api/weddingslideshow.api directory
* `dotnet lambda deploy-serverless` - publishes the infrastructure to AWS with cloudformation. Requires IAM credentials in a profile named lukeryannetnz.

### Front end publish to S3
Run these commands inside the root directory
* `aws s3 cp . s3://[BUCKETNAME] --recursive --exclude "*/**.*" --exclude ".git/*" --exclude ".DS_Store" --exclude ".gitignore" --exclude ".gitattributes"` - publishes the front end to an S3 bucket. Requires IAM credentials in the default profile.