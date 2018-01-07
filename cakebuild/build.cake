#load "tasks/publish.cake"

var target = Argument("target", "Publish");
var configuration = Argument("configuration", "Release");

RunTarget(target);