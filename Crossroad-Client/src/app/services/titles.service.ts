import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import ITitleInfo from '../shared/models/ITitleInfo';
import { environment } from '../../environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class TitlesService {
  
  public API = environment.crossroadApiUrl;
  public TITLES_API = `${this.API}/SearchByTitle`;
  public TITLES_API_DETAILS = `${this.API}/GetTitleDetails`;

  constructor(private http: HttpClient) {}

  //Get all titles, chld details information is not pulled by this method
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
    
    //Get all child details for a specific movie title
    //Gets information about Genres, Participants, Story lines, etc...
    public getTitleDetails(titleId: number) : Observable<ITitleInfo[]> {

      let url = this.TITLES_API_DETAILS + `?titleId=${titleId}`;
      return this.http.get(url).
      pipe(
         map((data: any) => {
           return data;
         }), catchError( error => {           
           return throwError(error.error);
         })
      )
    } 
}
