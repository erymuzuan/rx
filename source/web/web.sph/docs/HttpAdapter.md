#Using HTTP Adapter


Http adapter allows you to create an adapter to connected to any web application where these 2 conditions

* When you don't have access to the backend database
* When you want to preserve the existing application logic

It works as a proxy to forward any request you made to the existing HTTP endpoint, acting as a browser

HttpAdapter uses `System.Net.HttpClient` internally to manage HTTP request to the remote server