# TwitchVideosUtil

This is a WIP utility for querying available Twitch VODs.

## Get an auth token

Visit https://id.twitch.tv/oauth2/authorize?response_type=token&client_id=uxj8hdpst8v4lutkr842b3lxz8tp0o&redirect_uri=http://localhost:3000&scope=

The browser will redirect to a url like http://localhost:3000/#access_token=YOUR_TOKEN_HERE&scope=&token_type=bearer

Copy the token value from the url as indicated above and keep it secret.

## Get list of VODs

At a terminal, run the GetVideosList command, with the token (see above) and the username of the channel to fetch the video list from.

`.\TwitchVideosUtil.exe GetVideosList <TOKEN> <USERNAME>`

The video titles will be printed in date descending order in the format:

`YYMMDD - Title`
