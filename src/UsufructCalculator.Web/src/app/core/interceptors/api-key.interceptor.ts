import { HttpInterceptorFn } from '@angular/common/http';
import { environment } from '../../../environments/environment';

export const apiKeyInterceptor: HttpInterceptorFn = (req, next) => {
  // Only add API key to requests going to our API
  if (req.url.startsWith(environment.apiUrl)) {
    const clonedRequest = req.clone({
      setHeaders: {
        'X-API-Key': environment.apiKey
      }
    });
    return next(clonedRequest);
  }

  return next(req);
};
