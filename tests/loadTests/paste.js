import http from 'k6/http'
import { check, sleep } from 'k6'
import { Counter } from 'k6/metrics'

const countOfFailure = new Counter('countOfFailure');
export const options = {
    vus: 460,
    duration: '1s'
}

export default function () {
    const data = {
        content: 'string',
        password: 'string',
        expiresOn: '2025-03-28T15:13:20.090Z'
    };

    const params = {
        headers: {
            'Content-Type': 'application/json'
        }
    };

    let response = http.post('http://localhost:5081/texts/paste', JSON.stringify(data), params);
    var isRequestSucceeded = check(response, { 'succeeded': (r) => r.status === 200 });
    if (!isRequestSucceeded) {
        countOfFailure.add(1);
    }

    sleep(0.1);
}