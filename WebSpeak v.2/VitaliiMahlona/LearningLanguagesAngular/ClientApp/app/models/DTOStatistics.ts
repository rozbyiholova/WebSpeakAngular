import { DTO } from './DTO'
import { DTOTestResults } from './DTOTestResults' 
import { DTOTotalRating } from './DTOTotalRating'

export class DTOStatistics {
    public testResults: DTOTestResults[];
    public langList: DTO[];
    public catList: DTO[];
    public subCatList: DTO[];
    public totalRatings: DTOTotalRating[];
    public langRating: DTOTotalRating[];
}
