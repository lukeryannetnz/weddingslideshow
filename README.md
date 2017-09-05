# weddingslideshow
Displays a slideshow of images for a wedding

# weddingslideshow.api
This is a netcore 1.0 app that connects to the flickr API to pull down images by hashtag.

## Local development commands
Run these commands inside the ~/weddingslideshow.api directory
* `dotnet build` - compiles the solution
* `dotnet test` - runs the tests

## Deployment
Run these commands inside the ~/weddingslideshow.api/weddingslideshow.api directory
* `dotnet lambda deploy-serverless` - publishes the infrastructure to AWS with cloudformation. Requires IAM credentials in a profile named lukeryannetnz.