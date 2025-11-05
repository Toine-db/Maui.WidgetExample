//
//  File.swift
//  XCodeWidgetExample
//
//  Created by Toine Boer on 05/11/2025.
//

import WidgetKit
import SwiftUI

extension ConfigurationAppIntent {
    static var smiley: ConfigurationAppIntent {
        let intent = ConfigurationAppIntent()
        intent.favoriteEmoji = "😀"
        return intent
    }
    
    static var starEyes: ConfigurationAppIntent {
        let intent = ConfigurationAppIntent()
        intent.favoriteEmoji = "🤩"
        return intent
    }
}
