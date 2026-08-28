import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class GraphService {

   private apiUrl = 'https://localhost:7219/api/Database';

  constructor(private http: HttpClient) {}

  getProjectsByTechnology(
    technology: string
  ): Observable<any> {

    const params = new HttpParams()
      .set('technology', technology);

    return this.http.get(
      `${this.apiUrl}/projects-by-technology`,
      { params }
    );
  }

  getTechnologyDomains(
    technology: string
  ): Observable<any> {

    const params = new HttpParams()
      .set('technology', technology);

    return this.http.get(
      `${this.apiUrl}/technology-domains`,
      { params }
    );
  }

  getRecommendations(
    technology: string
  ): Observable<any> {

    const params = new HttpParams()
      .set('technology', technology);

    return this.http.get(
      `${this.apiUrl}/recommendations`,
      { params }
    );
  }
  getGraph(
  technology: string
): Observable<any> {

  const params = new HttpParams()
    .set('technology', technology);

  return this.http.get(
    `${this.apiUrl}/graph`,
    { params }
  );
}
}
