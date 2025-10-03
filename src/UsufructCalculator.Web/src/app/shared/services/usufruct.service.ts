import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UsufructRequest, UsufructResponse } from '../models/usufruct.models';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UsufructService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/v1/usufruct-calculations`;

  calculate(request: UsufructRequest): Observable<UsufructResponse> {
    const params = new HttpParams()
      .set('amount', request.amount.toString())
      .set('age', request.age.toString())
      .set('gender', request.gender.toString())
      .set('calculationMethod', request.calculationMethod.toString());

    return this.http.get<UsufructResponse>(this.apiUrl, { params });
  }
}
