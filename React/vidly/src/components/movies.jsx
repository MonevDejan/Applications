import React, { Component } from "react";
import { getMovies } from "../services/fakeMovieService";
import Pagination from "./common/pagination";
import paginate from "./../utilities/paginate";
import { ListGroup } from "./listGroup";
import { getGenres } from "./../services/fakeGenreService";
import MoviesTable from "./moviesTable";
import _ from "lodash";

export class Movies extends Component {
  state = {
    movies: [],
    genre: [],
    renderMoviesPerPage: 4,
    activePage: 1,
    selectedGenre: {},
    sortColumn: { path: "title", order: "asc" }
  };

  componentDidMount() {
    const movies = getMovies();
    const genre = [{ _id: "", name: "All movies" }, ...getGenres()];
    this.setState({ movies, genre, selectedGenre: genre[0] });
  }

  handrelDelete = movie => {
    const movies = this.state.movies.filter(m => m._id !== movie._id);
    this.setState({ movies });
  };

  handleLike = movie => {
    const movies = [...this.state.movies];
    const index = movies.indexOf(movie);
    movies[index] = { ...movie };
    movies[index].liked = !movies[index].liked;
    this.setState({ movies });
    //ToDo: code for updating the database
  };

  handlePageSelect = selectedPage => {
    this.setState({ activePage: selectedPage });
  };

  handleGenreSelect = genre => {
    this.setState({ selectedGenre: genre, activePage: 1 });
  };

  handleSort = sortColumn => {
    this.setState({ sortColumn });
  };

  getPagedData = () => {
    const {
      activePage,
      renderMoviesPerPage,
      movies: allMovies,
      selectedGenre,
      sortColumn
    } = this.state;

    const filterMovies =
      selectedGenre.name === "All movies"
        ? allMovies
        : allMovies.filter(movie => movie.genre.name === selectedGenre.name);
    const sorted = _.orderBy(
      filterMovies,
      [sortColumn.path],
      [sortColumn.order]
    );

    const movies = paginate(sorted, activePage, renderMoviesPerPage);

    return { totalCount: filterMovies.length, movies };
  };

  render() {
    const {
      activePage,
      renderMoviesPerPage,
      genre,
      selectedGenre,
      sortColumn
    } = this.state;

    const { totalCount, movies } = this.getPagedData();

    if (totalCount === 0) return <p>There are no movies in the database</p>;

    return (
      <div className="row">
        <div className="col-3">
          <ListGroup
            item={genre}
            onItemSelect={this.handleGenreSelect}
            selectedItem={selectedGenre}
          />
        </div>
        <div className="col-9">
          <p>There are total of {totalCount} movies in the database</p>

          <MoviesTable
            movies={movies}
            sortColumn={sortColumn}
            onLike={this.handleLike}
            onDelete={this.handrelDelete}
            onSort={this.handleSort}
          />

          <Pagination
            itemsNumber={totalCount}
            itemsPerPage={renderMoviesPerPage}
            onPageSelect={this.handlePageSelect}
            activePage={activePage}
          />
        </div>
      </div>
    );
  }
}
