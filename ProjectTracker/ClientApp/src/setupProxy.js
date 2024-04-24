const { createProxyMiddleware } = require('http-proxy-middleware');
const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:28737';

const context = [
  "/weatherforecast",
  "/user",
  "/api/project",
  "/user/login",
  "/api/tasks/project",
  "/api/userproject",
  "/api/tasks",
  "/api/status",
];

module.exports = function (app) {
  console.log("This is the context" + context);
  const appProxy = createProxyMiddleware(context, {
    target: target,
    secure: false,
    headers: {
      Connection: 'Keep-Alive'
    },
    headers: {
      'Content-Type': 'application/json'
    }
  });

  app.use(appProxy);
};
