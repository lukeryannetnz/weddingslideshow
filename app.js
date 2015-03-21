// this code was hacked together to get a prototype working. should be re-written with a framework. or at least split into separate files!
var DataPollFrequency = 120000;
var ImageSwapFrequency = 30000;

function ImageData()
{
  var that = this;
  this.imageUris = [];

  this.LoadImages = function(tag, recursionDepth, waterMark) {
    var searchuri;

    if(waterMark){
      console.log('recursing: ' + recursionDepth + ".")
      searchuri = "https://api.instagram.com/v1/tags/" + tag + "/media/recent?access_token=" + accessToken + "&max_tag_id=" + waterMark + "&callback=?";

      } else {
      console.log('polling.')
      searchuri = "https://api.instagram.com/v1/tags/" + tag + "/media/recent?access_token=" + accessToken + "&callback=?";
    }

    $.getJSON(searchuri,
      function(response) {
          for(var i = 0; i < response.data.length; i++) {
            if(response.data[i].images.standard_resolution.url
              && !lookup(that.imageUris, "uri", response.data[i].images.standard_resolution.url)){
                  that.imageUris.push({ uri: response.data[i].images.standard_resolution.url, dateLoaded : new Date()});
            }
            else{
              recursionDepth = 0;
            }
          }

          console.log('image count: ' + that.imageUris.length)

          if(recursionDepth > 0 && response.pagination.next_max_tag_id) {
            recursionDepth--;
            that.LoadImages(tag, recursionDepth, response.pagination.next_max_tag_id);
          }
      })
  }
}

// looks for a value as a property of an object in an array
function lookup(array, prop, value) {
    for (var i = 0; i < array.length; i++){
        if (array[i][prop] === value) {
          return array[i];
        }
    }
}

//dirty global scope for now
var accessToken;

function GoButtonHander(imageData){
  var tag = $("#hashtag").val();

  imageData.data.LoadImages(tag, 50);
  window.setInterval(imageData.data.LoadImages, DataPollFrequency, tag, 10)
  window.setTimeout(UpdateImageSrc, 1100, imageData.data);
  window.setInterval(UpdateImageSrc, ImageSwapFrequency, imageData.data)
  FullscreenImage();
  $(window).resize(FullscreenImage);
}

function RedirectToLogin(){
  window.location = "https://instagram.com/oauth/authorize/?client_id=834dd36a346648bdb999084cd10c3c79&redirect_uri=http://weddingslideshow.azurewebsites.net&response_type=token"
}

function TryGetAccessToken(){
  var token;

  if(location.hash){
    token = location.hash.split('=')[1];
    location.hash = '';
  }

  return token;
}

function ShowLoginButton(){
  $("#login").removeClass("hidden");
}

function ShowSearchBox(){
  $("#search").removeClass("hidden");
}

function HideSearchBox(){
  $("#search").addClass("hidden");
}

function getRandomInt(min, max) {
  return Math.floor(Math.random() * (max - min + 1)) + min;
}

function UpdateImageSrc(imageData){
  if(imageData.imageUris && imageData.imageUris.length > 0){
    var newImageUri = "";

    var lastImage = imageData.imageUris[imageData.imageUris.length -1]
    var lastSwapDate = new Date(new Date().getTime() - ImageSwapFrequency);
    if(lastImage && lastImage.dateLoaded > lastSwapDate){
      // if there is a new image, use that
      newImageUri = lastImage.uri;
    }
    else{
      // get a random image
      var index = getRandomInt(0, imageData.imageUris.length - 1);
      newImageUri = imageData.imageUris[index].uri;
    }

    $("#photo").attr("src",newImageUri);
  }
}

function FullscreenImage(){
  var $img = $('#photo'),
  imageWidth = $img[0].width, //need the raw width due to a jquery bug that affects chrome
  imageHeight = $img[0].height, //need the raw height due to a jquery bug that affects chrome
  maxWidth = $(window).width(),
  maxHeight = $(window).height(),
  widthRatio = maxWidth / imageWidth,
  heightRatio = maxHeight / imageHeight;

  var ratio = widthRatio; //default to the width ratio until proven wrong

  if (widthRatio * imageHeight > maxHeight) {
    ratio = heightRatio;
  }

  //now resize the image relative to the ratio
  $img.attr('width', imageWidth * ratio)
  .attr('height', imageHeight * ratio);

  //and center the image vertically and horizontally
  $img.css({
    margin: 'auto',
    position: 'absolute',
    top: 0,
    bottom: 0,
    left: 0,
    right: 0
  });

  HideSearchBox();
}

$(document).ready(function(){
  $("#login").click(RedirectToLogin);

  accessToken = TryGetAccessToken();
  if(!accessToken)
  {
    ShowLoginButton();
  }
  else
  {
    ShowSearchBox();
  }

  var imageData = new ImageData();
  $("#go").click(imageData, GoButtonHander);
})
