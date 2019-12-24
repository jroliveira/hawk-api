local BasePlugin = require "kong.plugins.base_plugin"

local EchoHandler = BasePlugin:extend()

EchoHandler.PRIORITY = 2000
EchoHandler.VERSION = "0.1.0"

function EchoHandler:new()
  EchoHandler.super.new(self, "header-echo")

  self.echo_string = ""
end

-- Run this when the client request hits the service
function EchoHandler:access(conf)
  EchoHandler.super.access(self)

  -- kong.* functions are from the PDK (plugin development kit)
  -- and do not need to be explicitly required
  self.echo_string = kong.request.get_header(conf.requestHeader)
end

-- Run this when the response header has been received
-- from the upstream service
function EchoHandler:header_filter(conf)
  EchoHandler.super.header_filter(self)

  if self.echo_string ~= "" then
    kong.response.set_header(conf.responseHeader, self.echo_string)
  end
end

return EchoHandler