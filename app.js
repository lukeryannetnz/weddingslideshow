$(document).ready(function(){
  $("#login").click(function(){
    window.location = "https://instagram.com/oauth/authorize/?client_id=834dd36a346648bdb999084cd10c3c79&redirect_uri=http://weddingslideshow.azurewebsites.net&response_type=token"
  })

  var accessToken;

  if(location.hash)
  {
    accessToken = location.hash.split('=')[1];
    console.log('access token: '.concat(accessToken));
    location.hash = '';
  }

  $("#go").click(function(){
    var tag = $("#hashtag").val();
    var searchuri = "https://api.instagram.com/v1/tags/" + tag + "/media/recent?access_token=" + accessToken;
    console.log(searchuri);
    $().get(searchuri, { count : "5" })
      .done(function(data){
          console.log(data);
        })
  })
})
