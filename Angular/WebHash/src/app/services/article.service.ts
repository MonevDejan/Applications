import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Article } from '../models/article.model';
import { Observable } from 'rxjs';

const httpOptions = {
  headers: new HttpHeaders({
      'Content-Type': 'application/json'
  })
};

@Injectable({
  providedIn: 'root'
})
export class ArticleService {
  constructor(private http: HttpClient) { }
  getAll() : Observable<Article[]> {
    return this.http.get<Article[]>('/api/Articles/Get', httpOptions);
  }
  getById(id: number) : Observable<Article> {
      return this.http.get<Article>('/api/Articles/GetById/' + id);
  }

  add(item: Article) {

      return this.http.post<Article>('/api/Articles/Add', item, httpOptions);
  }

  update(item: Article) {
      return this.http.post('/api/Articles/Update/', item, httpOptions);
  }

  delete(id) {
      return this.http.post('/api/Articles/Delete/' + id, httpOptions);
  }


}
