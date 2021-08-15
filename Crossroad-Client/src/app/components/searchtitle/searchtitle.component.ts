import { Component, OnInit, Output, EventEmitter, ViewChild } from '@angular/core';
import { NgbCarouselConfig } from '@ng-bootstrap/ng-bootstrap';
import { TitlesService } from '../../services/titles.service';
import ITitleInfo from '../../shared/models/ITitleInfo';
import { Observable,  Subscription  } from 'rxjs';

@Component({
  selector: 'app-searchtitle',
  templateUrl: './searchtitle.component.html',
  styleUrls: ['./searchtitle.component.css'],
  providers: [NgbCarouselConfig]  
})
export class SearchTitleComponent implements OnInit {
    
  image =location.href.substring(0, location.href.lastIndexOf("/")+1) + "assets/images/LaPortraitTitle.jpg";  
  subscription: Subscription=new Subscription();
  subscriptionClearData: Subscription=new Subscription();
  moviesFound: boolean = false;  
  titleInfoResults: Observable<ITitleInfo[]> = new Observable<ITitleInfo[]>();
  objectKeys =new Array();   
  searchTerm: string='';  

  @Output() titleInputEvent: EventEmitter<string> = new EventEmitter();
 
  constructor(config: NgbCarouselConfig, 
              private titleService : TitlesService)   
  {
    config.interval = 3000;
    config.keyboard = true;    
    config.pauseOnHover=true;      
  }

  ngOnInit(): void {}

  ngOnDestroy() {
    this.subscription.unsubscribe();
    this.subscriptionClearData.unsubscribe();
  }

  changeTitle(value: string) {
    this.titleInputEvent.emit(value);    
    this.searchTerm=value;
  }

  clearSearch()
  {
    this.moviesFound=false;
   
  }
  viewTitleDetails()
  {
      alert("You can't view title details on this version of the application.");
  }
  resetSearchResults()
  {       
    this.objectKeys=new Array();      
  }

  searchTitles()
  {          
          this.moviesFound=true;          
          this.subscription= this.titleService.searchTitles(this.searchTerm).subscribe((data:any) => {  
          this.titleInfoResults=data as Observable<ITitleInfo[]>;  
         
          //We need an iterable object to be able to  
          //display title information on the template.
          this.objectKeys=JSON.parse(JSON.stringify(data));   
          
          //Reset search results label text
          const searchLabel = document.getElementById("searchResults");
          if (searchLabel!=null && searchLabel!=undefined){
            searchLabel.innerHTML="Search Results:";                                           
          } 
         },
         error => {
          this.resetSearchResults();
         },);                                    
  }

}
