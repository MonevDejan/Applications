import { Component, OnInit } from '@angular/core';
import { ArticleService } from '../services/article.service';
import { Article } from '../models/article.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-articles',
  templateUrl: './articles.component.html',
  styleUrls: ['./articles.component.scss']
})
export class ArticlesComponent implements OnInit {

  all: Article[];

  public bool: boolean = false;

  singleArticle: Article;

  constructor(private _articleService: ArticleService, private route: Router) { }

  ngOnInit() {
    this._articleService.getAll().subscribe(x => {
      this.all = x;
    });
  }
  
  getSingleArticle(id: number): void {
    this.route.navigateByUrl('/articles/' + id);
  }
}

// delete(event, id) {
//     event.stopPropagation();
//     if (confirm(this.translate.instant("General.AreYouSure"))) {

//         this.sectionService.delete(id).subscribe(data => {

//             const itemToDelete = this.all.findIndex(x => {
//                 return x.id === id;
//             });

//             this.messageService.setTheMessage(
//                 this.translate.instant("General.DeletedSuccessfully")
//                 , 'success');

//             this.all.splice(itemToDelete, 1);

//         });
//         return false;
//     }
// }


