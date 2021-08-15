import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  titleInput:string='';

  title = 'Crossroad-Client';  

  changeSearchTerm(inputValue: string){   
    this.titleInput="Title search: " + inputValue;
  } 
}
