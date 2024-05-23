import {Observable} from "rxjs";

export interface BaseEndpoint<TRequest, TResponse>{
  Handle(request: TRequest): Observable<TResponse>;
}
