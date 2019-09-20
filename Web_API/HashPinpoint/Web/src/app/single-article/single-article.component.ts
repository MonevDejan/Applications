import { Component, OnInit } from '@angular/core';
import { ArticleService } from '../services/article.service';
import { ActivatedRoute } from '@angular/router';
import { Article } from '../models/article.model';
import { Location } from '@angular/common';

@Component({
  selector: 'app-single-article',
  templateUrl: './single-article.component.html',
  styleUrls: ['./single-article.component.scss']
})
export class SingleArticleComponent implements OnInit {

  constructor(private _articleService : ArticleService, private activeRoute : ActivatedRoute, private location : Location) { }

  private _articleId : any;
  
  public article : Article;

  ngOnInit() {
    this._articleId = this.activeRoute.snapshot.paramMap.get('id');
    this._articleService.getById(this._articleId).subscribe(article => this.article = article);
  }

  goBack() {
    this.location.back();
  }

}
