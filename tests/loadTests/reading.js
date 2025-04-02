import http from 'k6/http'
import { check, sleep } from 'k6'
import { Counter } from 'k6/metrics'

const countOfFailure = new Counter('countOfFailure');
export const options = {
    vus: 460,
    duration: '1s'
}

export default function () {

    let response = http.get('http://localhost:5081/texts/c-68987995');
    var isRequestSucceeded = check(response, { 'succeeded': (r) => r.status === 200 });
    if (!isRequestSucceeded) {
        countOfFailure.add(1);
    }

    sleep(0.1);
}