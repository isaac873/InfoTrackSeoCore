import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { HttpClientModule } from '@angular/common/http';
import { SeoIndexComponent } from './seo-index/seo-index.component';

@NgModule({
  declarations: [
    AppComponent,
    SeoIndexComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    HttpClientModule
  ],
  exports: [
    SeoIndexComponent
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
