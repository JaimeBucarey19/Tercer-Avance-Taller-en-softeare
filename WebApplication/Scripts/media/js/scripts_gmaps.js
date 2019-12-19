$(function(){
    if (navigator.geolocation) {
      navigator.geolocation.getCurrentPosition(function(position) {
        var pos = {
          lat: position.coords.latitude,
          lng: position.coords.longitude
        };

        infoWindow.setPosition(pos);
        var marker = new google.maps.Marker({
         position: pos,
         map: map
       });
        infoWindow.setContent('Location found.');
        map.setCenter(pos);
      }, function() {
        handleLocationError(true, infoWindow, map.getCenter());
      });
    } else {
      // Browser doesn't support Geolocation
      handleLocationError(false, infoWindow, map.getCenter());
    }
                                }
                                function handleLocationError(browserHasGeolocation, infoWindow, pos) {
                                     infoWindow.setPosition(pos);
                                     infoWindow.setContent(browserHasGeolocation ?
                                                           'Error: The Geolocation service failed.' :
                                                           'Error: Your browser doesn\'t support geolocation.');
                                   }
    function getCoords(position){
        var pos = {
          lat: position.coords.latitude,
          lng: position.coords.longitude
        };
        inicialize(pos.lat,pos.lng);
    }
    function getError(err){
        inicialize();
    }
    function inicialize(lat,lng){
        
    }
}
