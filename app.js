$(document).ready(function(){
  $("#login").click(function(){
    window.location = "https://instagram.com/oauth/authorize/?client_id=834dd36a346648bdb999084cd10c3c79&redirect_uri=http://weddingslideshow.azurewebsites.net&response_type=token"
  })

  var accessToken = location.hash.split('=')[1];
  console.log('access token'.concat(accessToken));
  location.hash = null;
})
