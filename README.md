# weddingslideshow
Javascript application that displays a fullscreen slideshow of images for a wedding.

# weddingslideshow.api
This is a netcore 1.0 app that connects to the flickr API to pull down images by hashtag.

## Local development commands
Run these commands inside the ~/weddingslideshow.api directory
* `dotnet build` - compiles the solution
* `dotnet test` - runs the tests

## Deployment
### API publish via AWS SAM to cloudformation, api gateway, S3 and lambda
Run these commands inside the ~/weddingslideshow.api/weddingslideshow.api directory
* `dotnet lambda deploy-serverless` - publishes the infrastructure to AWS with cloudformation. Requires IAM credentials in a profile named lukeryannetnz.
Run these commands inside the root directory
### Front end publish to S3
* `aws s3 cp . s3://[BUCKETNAME] --recursive --exclude "*/**.*" --exclude ".git/*" --exclude ".DS_Store" --exclude ".gitignore" --exclude ".gitattributes"` - publishes the front end to an S3 bucket. Requires IAM credentials in the default profile.