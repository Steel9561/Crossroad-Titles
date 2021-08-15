import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError, Subject, BehaviorSubject, Subscription } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import ITitleInfo from '../shared/models/ITitleInfo';
import { environment } from '../../environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class TitlesService {
  
  public API = environment.crossroadApiUrl;
  public TITLES_API = `${this.API}/SearchByTitle`;
  constructor(private http: HttpClient) {}

  public searchTitles(searchTerm: string) : Observable<ITitleInfo[]> {

   let url = this.TITLES_API + `?titleName=${searchTerm}`;
    return this.http.get(url).
        pipe(
           map((data: any) => {
             return data;
           }), catchError( error => {
             const searchLabel = document.getElementById("searchResults");
             if (searchLabel!=null && searchLabel!=undefined){
                searchLabel.innerHTML="No search results. Please try another title.";                                           
             }
             return throwError(error.error);
           })
        )
    }     
}
