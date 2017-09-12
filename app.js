// this code was hacked together to get a prototype working. should be re-written with a framework. or at least split into separate files!
var DataPollFrequency = 90000;
var ImageSwapFrequency = 30000;

function ImageData()
{
  var that = this;
  this.imageUris = [];

  this.LoadImages = function(tag, recursionDepth, waterMark) {
    var searchuri;

    if(waterMark){
      console.log('recursing: ' + recursionDepth + ".")
      searchuri = "http://localhost:5000/api/images?tag=" + tag + "&maxId=" + waterMark;

      } else {
      console.log('polling.')
      searchuri = "http://localhost:5000/api/images?tag=" + tag;
    }

    $.getJSON(searchuri,
      function(response) {
          for(var i = 0; i < response.data.length; i++) {
            if(response.data[i].location
              && !lookup(that.imageUris, "uri", response.data[i].location)){
                  that.imageUris.push({ uri: response.data[i].location, dateLoaded : new Date()});
            }
            else{
              recursionDepth = 0;
            }
          }

          console.log('image count: ' + that.imageUris.length)

          if(recursionDepth > 0 && response.pagination.nextMaxId) {
            recursionDepth--;
            that.LoadImages(tag, recursionDepth, response.pagination.nextMaxId);
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

  ShowLoading();
  imageData.data.LoadImages(tag, 50);
  window.setInterval(imageData.data.LoadImages, DataPollFrequency, tag, 10)
  window.setTimeout(UpdateImageSrc, 3000, imageData.data);
  window.setInterval(UpdateImageSrc, ImageSwapFrequency, imageData.data)
  FullscreenImage();
  $(window).resize(FullscreenImage);
}

function ShowSearchBox(){
  $("#search").removeClass("hidden");
}

function HideSearchBox(){
  $("#search").addClass("hidden");
}

function ShowLoading(){
  $("#loading").removeClass("hidden");
}

function HideLoading(){
  $("#loading").addClass("hidden");
}

function getRandomInt(min, max) {
  return Math.floor(Math.random() * (max - min + 1)) + min;
}

function UpdateImageSrc(imageData){
  if(imageData.imageUris && imageData.imageUris.length > 0){
    var newImageUri = "";

    var lastImage = imageData.imageUris[imageData.imageUris.length -1]
    var lastSwapDate = new Date(new Date().getTime() - (ImageSwapFrequency + 20000));
    if(lastImage && lastImage.dateLoaded >= lastSwapDate){
      // if there is a new image since the last time we swapped, use that
      newImageUri = lastImage.uri;
      console.log("preferring new image " + newImageUri)
    }
    else{
      // get a random image
      var index = getRandomInt(0, imageData.imageUris.length - 1);
      newImageUri = imageData.imageUris[index].uri;
      console.log("displaying random image " + newImageUri)
    }

    HideLoading();
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
  ShowSearchBox();

  var imageData = new ImageData();
  $("#go").click(imageData, GoButtonHander);
})
