
function LoadImages(accessToken)
{
  var tag = $("#hashtag").val();
  var searchuri = "https://api.instagram.com/v1/tags/" + tag + "/media/recent?access_token=" + accessToken + "&callback=?";
  console.log(searchuri);
  $.getJSON(searchuri, function(response){
    console.log(response);
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

  var accessToken = TryGetAccessToken();
  if(!accessToken)
  {
    ShowLoginButton();
  }
  else
  {
    ShowSearchBox();
  }

  $("#go").click(accessToken, LoadImages);
})
