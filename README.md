# EagleRock

Getting started. Need Redis installed.
For me I'm just running Redis in docker via:
```
docker run -d --cap-add sys_resource --name rp -p 8443:8443 -p 9443:9443 -p 12000:12000 redislabs/redis
```