Task("Up-Dependencies")
    .Does(() => DockerComposeUp(
        new DockerComposeUpSettings
        {
            Files = new [] { "docker-compose.yml" },
            AbortOnContainerExit = true,
        },
        "swagger",
        "graphdb",
        "grafana",
        "prometheus",
        "kibana",
        "filebeat",
        "logstash",
        "elasticsearch"));
