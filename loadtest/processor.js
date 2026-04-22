module.exports = {
  beforeRequest: (requestParams, context, ee, next) => {
    requestParams.headers['Content-Type'] = 'application/json';
    return next();
  }
};
