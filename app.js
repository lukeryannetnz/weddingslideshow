//dirty global scope for now
var accessToken;

function ImageData(imageUris, minTagId)
{
  this.imageUris = imageUris;
  this.maxTagId = minTagId;
}

function LoadImages(imageData)
{
  var tag = $("#hashtag").val();
  var searchuri = "https://api.instagram.com/v1/tags/" + tag + "/media/recent?access_token=" + accessToken + "&callback=?";
  console.log(searchuri);
  $.getJSON(searchuri,
  { count : "10", maxtagid : imageData.data.minTagId},
    function(response){
      console.log(response);
      for(var i = 0; i < response.data.length; i++){
        if(response.data[i].images.standard_resolution.url)
        {
          imageData.data.minTagId = response.data[i].id
          imageData.data.imageUris.push(response.data[i].images.standard_resolution.url)
          $("#photo").attr("src",response.data[i].images.standard_resolution.url)
        }
      }
      console.log(imageData);
  })
}

function RedirectToLogin()
{
  window.location = "https://instagram.com/oauth/authorize/?client_id=834dd36a346648bdb999084cd10c3c79&redirect_uri=http://weddingslideshow.azurewebsites.net&response_type=token"
}

function TryGetAccessToken()
{
  var token;

  if(location.hash)
  {
    token = location.hash.split('=')[1];
    console.log('access token: '.concat(token));
    location.hash = '';
  }

  return token;
}

function ShowLoginButton()
{
  $("#login").removeClass("hidden");
}

function ShowSearchBox()
{
  $("#search").removeClass("hidden");
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

  $("#go").click(new ImageData([], 0), LoadImages);
})
