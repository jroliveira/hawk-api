Task("Up-Dependencies")
    .Does(() => DockerComposeUp(
        new DockerComposeUpSettings
        {
            Files = new [] { "docker-compose.yml" },
            AbortOnContainerExit = true,
        },
        "swagger",
        "grafana",
        "prometheus",
        "graphdb",
        "kibana",
        "filebeat",
        "logstash",
        "elasticsearch",
        "jaegertracing",
        "kong-gui",
        "kong"));
