import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { NgbCarouselConfig } from '@ng-bootstrap/ng-bootstrap';
import { TitlesService } from '../../services/titles.service';
import {  Subscription   } from 'rxjs';
import {NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-searchtitle',
  templateUrl: './searchtitle.component.html',
  styleUrls: ['./searchtitle.component.css'],
  providers: [NgbCarouselConfig]  
})
export class SearchTitleComponent implements OnInit {
    
  image =location.href.substring(0, location.href.lastIndexOf("/")+1) + "assets/images/LaPortraitTitle.jpg";  
  subscription: Subscription=new Subscription();  
  subscriptionTitleDetails: Subscription=new Subscription();
  moviesFound: boolean = false;   
  objectKeys =new Array();    
  searchTerm: string='';  

  //Arrays for loading title related details
  objectKeysDetailsGenres =new Array();   
  objectKeysDetailsAwards =new Array();   
  objectKeysDetailsParticipants =new Array();   
  objectKeysDetailsOtherNames =new Array();   
  objectKeysDetailsStoryLines =new Array();   

  @Output() titleInputEvent: EventEmitter<string> = new EventEmitter();
 
  constructor(config: NgbCarouselConfig, 
              private titleService : TitlesService,
              private modalService: NgbModal)   
  {
    config.interval = 3000;
    config.keyboard = true;    
    config.pauseOnHover=true;      
  }

  ngOnInit(): void {}

  ngOnDestroy() {
    this.subscription.unsubscribe();    
    this.subscriptionTitleDetails.unsubscribe();
  }

  viewTitleDetails(content:any, titleId: number, titleName: string) {
    this.getTitleDetails(titleId);            
    this.modalService.open(content, {ariaLabelledBy: 'modal-basic-title'});
    
    //Show movie title on modal window
    const movieDetailsHeader = document.getElementById("modal-basic-title");
    if (movieDetailsHeader!=null && movieDetailsHeader!=undefined){
      movieDetailsHeader.innerHTML=titleName;                                           
    } 
  }

  getTitleDetails(titleId: number)
  {
    this.subscriptionTitleDetails= this.titleService.getTitleDetails(titleId).subscribe((data:any) => {  
   
      //We need an iterable object to be able to  
      //display title information on the template.      
      this.objectKeysDetailsAwards=
                      JSON.parse(JSON.stringify(data[0]["titles"]["awards"]));         
      this.objectKeysDetailsParticipants=
                      JSON.parse(JSON.stringify(data[0]["titles"]["titleParticipants"]));         
      this.objectKeysDetailsOtherNames=
                      JSON.parse(JSON.stringify(data[0]["titles"]["otherNames"]));         
      this.objectKeysDetailsGenres=
                      JSON.parse(JSON.stringify(data[0]["titles"]["titleGenres"]));         
      
      this.objectKeysDetailsStoryLines=
                      JSON.parse(JSON.stringify(data[0]["titles"]["storyLines"]));         
                  
     },
     error => {},);  
  }

  changeTitle(value: string) {
    this.titleInputEvent.emit(value);    
    this.searchTerm=value;
  }

  clearSearch(inputElement: HTMLInputElement): void 
  {
    this.moviesFound=false;
    inputElement.value='';

    const typedLabel = document.getElementById("typedTitle");
    if (typedLabel!=null && typedLabel!=undefined){
      this.changeTitle('');                                                                                  
    } 
    
    this.adjustLayout(false);
  }
 
  resetSearchResults()
  {       
    this.objectKeys=new Array();      
  }

  adjustLayout(searchedTitle: boolean)
  {  
    const jumbotronSearch = document.getElementById("mainSearchArea");
    const divBottom = document.getElementById("divBottom");
 
    if (searchedTitle==true){        
        if (jumbotronSearch!=null && jumbotronSearch!=undefined){
          jumbotronSearch.style.top="22%";          
        } 

        if (divBottom!=null && divBottom!=undefined){
          divBottom.style.top="20%";
          divBottom.style.marginTop="3em";
          divBottom.style.paddingTop="160px";
        } 
    }
    else
    {
        if (jumbotronSearch!=null && jumbotronSearch!=undefined){
          jumbotronSearch.style.top="50%";
        }        
    }   
  }

  searchTitles()
  {          
    this.resetSearchResults();
        this.moviesFound=true; 
           
        this.subscription= this.titleService.searchTitles(this.searchTerm).subscribe((data:any) => {           
         
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
                  
         this.adjustLayout(true);
         
  }

}
