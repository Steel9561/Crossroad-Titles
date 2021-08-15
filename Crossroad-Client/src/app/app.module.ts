import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { SearchTitleComponent } from './components/searchtitle/searchtitle.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { TitlesService } from './services/titles.service';
import { HttpClientModule } from '@angular/common/http'; 

@NgModule({
  declarations: [
    AppComponent,
    SearchTitleComponent
  ],
  imports: [
    BrowserModule,    
    NgbModule,
    HttpClientModule  
  ],
  providers: [TitlesService],
  bootstrap: [AppComponent]
})
export class AppModule { }
