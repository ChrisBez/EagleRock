# EagleRock

As per the specs in the PDF. This API handles the upload of traffic data, retrieval of the latest traffic stats, and has a WIP RabbitMQ layer. The API can be tested via the /swagger page hosted in it.

To run this application, You'll need to install Redis.
You can get Redis running by running the following docker:
```
docker run -d --cap-add sys_resource --name rp -p 8443:8443 -p 9443:9443 -p 12000:12000 redislabs/redis
```

The demo app is not 100%, here's some things I would have liked to have finished.
1. Improved Test coverage, would have liked to get that up higher for the demo. Big ticket things to test would be the RabbitMQ layer, and the data retrival feature.
2. Have my RabbitMQ implementation working with a real RabbitMQ hosted in Docker. Didn't get that far so I wrapped my RabbitMQ calls in a feature management flag
3. Dig into Redis retrieval best practices a bit more. I think there's a better approach out there compared to the one I implemented
4. Added in a /healthcheck route that checks and reports on the health of the API (e.g. If Redis and RabbitMQ are running etc.)
5. Worked on the things I've flagged with TODO in the source code
