import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
//import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ArticlesComponent } from './articles/articles.component';
import { ArticleService } from './services/article.service';

import { HttpClientModule } from '@angular/common/http';
import { FeedConfigurationComponent } from './feed-configuration/feed-configuration.component';
import { ManageNewsComponent } from './manage-news/manage-news.component';
import { SignInComponent } from './sign-in/sign-in.component';
import { SingleArticleComponent } from './single-article/single-article.component';

@NgModule({
  declarations: [
    AppComponent,
    ArticlesComponent,
    FeedConfigurationComponent,
    ManageNewsComponent,
    SignInComponent,
    SingleArticleComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [ArticleService],
  bootstrap: [AppComponent]
})
export class AppModule { }
