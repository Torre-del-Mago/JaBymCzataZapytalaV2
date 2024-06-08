import {Observable, pipe, map, timer, flatMap} from 'rxjs'
import {AdminInfo} from './dto/real-time/AdminInfo'

export function realTimeObservable<T>(getInfoFunction: () => Observable<T>, time: number = 5000): Observable<T> {
    return timer(0, time).pipe(
        flatMap((_) => {
            return getInfoFunction()
        })
    )
}