import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { InfoTrackSeoModel } from 'src/models/infotrack-seo-model';

import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-seo-index',
  templateUrl: './seo-index.component.html',
  styleUrls: ['./seo-index.component.css']
})
export class SeoIndexComponent implements OnInit {

  constructor(private _httpClient: HttpClient) { }

  public result: number[] = [];
  public errorMessage = '';

  ngOnInit() {
    
    this._httpClient.get<InfoTrackSeoModel>(environment.url + '/api/SEO/get-results').subscribe(result => {
      if (result.success) {
        this.result = result.indexPositions;
      } else {
        this.errorMessage = result.message;
      }
    }, () => {
      this.errorMessage = "The service could not be loaded. Please try again later."
    });
  }
}
