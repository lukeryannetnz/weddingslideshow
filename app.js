// this code was hacked together to get a prototype working. should be re-written with a framework. or at least split into separate files!
var DataPollFrequency = 120000;
var ImageSwapFrequency = 30000;

// datastructure for image uris
function ImageData(imageUris, maxTagId)
{
  this.imageUris = imageUris;
}

//dirty global scope for now
var imageData = new ImageData([], 0);
var accessToken;

function LoadImages(imageData, recursionDepth, waterMark) {
  var tag = $("#hashtag").val();
  var searchuri;

  if(waterMark){
    console.log('recursing. ' + recursionDepth + ' image count: ' + imageData.imageUris.length)
    searchuri = "https://api.instagram.com/v1/tags/" + tag + "/media/recent?access_token=" + accessToken + "&max_tag_id=" + watermark + "&callback=?";

    } else {
    console.log('polling. image count: ' + imageData.imageUris.length)
    searchuri = "https://api.instagram.com/v1/tags/" + tag + "/media/recent?access_token=" + accessToken + "&callback=?";
  }

  $.getJSON(searchuri,
    function(response) {
        for(var i = 0; i < response.data.length; i++) {
          if(response.data[i].images.standard_resolution.url &&
            $.inArray(response.data[i].images.standard_resolution.url, imageData.imageUris) < 0){
                imageData.imageUris.push(response.data[i].images.standard_resolution.url);
          }
        }

        if(recursionDepth > 0) {
          recursionDepth--;

          //poll but not too quickly
          window.setTimeout(LoadImages(imageData, recursionDepth, response.pagination.next_max_tag_id), 200);
        }
    })
}

function GoButtonHander(imageData){
  LoadImages(imageData.data, 10);
  window.setInterval(LoadImages, DataPollFrequency, imageData.data, 10)
  window.setTimeout(UpdateImageSrc, 1000);
  window.setInterval(UpdateImageSrc, ImageSwapFrequency)
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

function UpdateImageSrc(){
  if(imageData.imageUris && imageData.imageUris.length > 0){
    var index = getRandomInt(0, imageData.imageUris.length - 1);
    $("#photo").attr("src",imageData.imageUris[index]);
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

  $("#go").click(imageData, GoButtonHander);
})
