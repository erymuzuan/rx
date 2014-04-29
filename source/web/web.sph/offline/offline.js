  $(function(){
        $('header#header').load('header.html');

        window.applicationCache.addEventListener('updateready', function(e){
        	if(window.applicationCache.status === window.applicationCache.UPDATEREADY){
        		window.applicationCache.swapCache();
        		if(window.confirm('New version has been downloaded, Do you want to reload the app?')){
        			window.location.reload();
        		}
        	}

        }, false);

 });